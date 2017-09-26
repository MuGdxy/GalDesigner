﻿using System;
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

        private class Sentence
        {
            public string Tag;
            public string FilePath;

            public static string Include(string input) => '"' + input + '"';

            public static string Unclude(string input) => input.Substring(1, input.Length - 2);

            public override string ToString()
            {
                return Tag + " = " + Include(FilePath);
            }
        }

        private const string SuffixName = "buildList";

        private Dictionary<string, ResourceAnalyser> resList;
        private Dictionary<string, ResourceAnalyser> configList;

        private void ProcessSentenceValue(ref string value, BlockType blockType, int Line, string FileTag)
        {
            Sentence sentence = new Sentence();

            //Build Sentence, left is Tag, right is FilePath
            var result = value.Split(new char[] { '=' }, 2);

            sentence.Tag = result[0]; sentence.FilePath = Sentence.Unclude(result[1]);

            switch (blockType)
            {
                case BlockType.Unknown:
                    DebugLayer.ReportError(ErrorType.InvaildFileType, Line, FileTag);
                    break;

                case BlockType.resList:
                    //In resList Block, create resListAnalyser and load it.
                    var resListAnalyser = new ResListAnalyser(sentence.Tag, sentence.FilePath);
                    resListAnalyser.LoadAnalyser();

                    resList.Add(sentence.Tag, resListAnalyser);
                    break;

                case BlockType.gsConfig:
                    //In config Block, create configAnalyser and load it.
                    var configAnalyser = new ConfigAnalyser(sentence.Tag, sentence.FilePath);
                    configAnalyser.LoadAnalyser();

                    configList.Add(sentence.Tag, configAnalyser);
                    break;

                default:
                    break;
            }

            value = "";
        }

        private void ProcessBlock(ref string blockContents, BlockType blockType, int Line, string FileTag)
        {
            bool inString = false;

            string currentString = "";

            foreach (var item in blockContents)
            {
                //new line
                if (item is '\n') { Line++; continue; }

                //find string, we need add " to our string
                if (item is '"') { currentString += item; inString ^= true; continue; }

                //Bulid String to making Sentence
                if (Utilities.IsAlphaOrNumber(item) is true || item is '=' || inString is true)
                {
                    currentString += item;
                    continue;
                }

                //Finish a sentence, we need to process it.
                if (item is ',')
                {
                    ProcessSentenceValue(ref currentString, blockType, Line, FileTag);
                    continue;
                }

            }

            //The end
            ProcessSentenceValue(ref currentString, blockType, Line, FileTag);

            blockContents = "";
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
           
            bool inCodeBlock = false;

            string currentBlock = "";
            string typeName = "";

            foreach (var item in contents)
            {
                //Build Block Contents
                if (inCodeBlock is true)
                    currentBlock += item;

                switch (item)
                {
                    case '\n':
                        //new line
                        line++;
                        break;

                    case '{':
                        //Block's begining
#if DEBUG
                        DebugLayer.Assert(inCodeBlock is true, ErrorType.InvalidResourceFormat, line, FilePath);
#endif
                        inCodeBlock = true;

                        //get the "{" 's line.
                        blockStartLine = line;
                        break;

                    case '}':
                        //Block's ending
#if DEBUG
                        DebugLayer.Assert(inCodeBlock is false, ErrorType.InvalidResourceFormat, line, FilePath);
#endif

                        BlockType blockType = GetBlockType(typeName, blockStartLine, Tag);

                        //Process a block
                        ProcessBlock(ref currentBlock, blockType, blockStartLine, Tag);

                        //clean up
                        typeName = "";
                        blockStartLine = 0;
                        inCodeBlock = false;
                        break;

                    default:
                        if (Utilities.IsAlpha(item) is true && inCodeBlock is false)
                            typeName += item;
                        break;
                }
            }

        }

        protected override void ProcessWriteFile(out string contents)
        {
            contents = "\n";
            
            //build resList block
            contents += "resList{\n";

            int line = 0;

            foreach (var item in resList)
            {
                line++;

                var sentence = new Sentence()
                {
                    Tag = item.Key,
                    FilePath = item.Value.FilePath
                };

                contents += "\t" + sentence;

                if (line < resList.Count) contents += ',';

                contents += "\n";
            }

            contents += "}\n\n\n";

            //build config Block
            contents += "gsConfig{\n";

            line = 0;

            foreach (var item in configList)
            {
                line++;

                var sentence = new Sentence()
                {
                    Tag = item.Key,
                    FilePath = item.Value.FilePath
                };

                contents += "\t" + sentence;

                if (line < configList.Count) contents += ',';

                contents += "\n";
            }

            contents += "}\n";

        }

        private static void ProcessDirectory(string directoryPath)
        {
            var files = System.IO.Directory.GetFiles(directoryPath);

            foreach (var item in files)
            {
                if (Utilities.GetFileSuffix(item) is SuffixName)
                {
                    var buildListAnalyser = new BuildListAnalyser(item, item);
                    buildListAnalyser.LoadAnalyser();
                }
            }

            var directorys = System.IO.Directory.GetDirectories(directoryPath);

            foreach (var item in directorys)
            {
                ProcessDirectory(item);
            }

        }

        public static void LoadAllBuildList()
        {
            ProcessDirectory(Environment.CurrentDirectory);
        }

    }
}
