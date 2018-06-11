using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class ScriptManager
    {
        private static List<Script>[] scripts = new List<Script>[(int)ScriptType.Count];

        private static Script CreateScript(ScriptType type, string content, int line, string filePath)
        {
            switch (type)
            {
                case ScriptType.Unknown:
                case ScriptType.Count:

                    DebugLayer.ReportError(ErrorType.InvaildScriptType, line, filePath);

                    return null;
                case ScriptType.Brush:

                    return new BrushScript(content, line, filePath);
                case ScriptType.Image:

                    return new ImageScript(content, line, filePath);
                case ScriptType.Voice:

                    return new VoiceScript(content, line, filePath);
                case ScriptType.TextFormat:

                    return new TextFormatScript(content, line, filePath);
                case ScriptType.VisualObject:

                    return new VisualObjectScript(content, line, filePath);
                case ScriptType.Script:

                    return new CommonScript(content, line, filePath);
                case ScriptType.Animation:

                    return new AnimationScript(content, line, filePath);
                case ScriptType.Animator:

                    return new AnimatorScript(content, line, filePath);
                case ScriptType.Scene:

                    return new SceneScript(content, line, filePath);
                case ScriptType.Config:

                    return new ConfigScript(content, line, filePath);
                default:
                    return null;
            }
        }

        private static void ProcessScriptFile(ScriptFile script)
        {
            var code = script.Code;

            //true is in block, false is not.
            bool inBlock = false;
            bool inString = false;

            int currentLine = 1;

            int blockStartLine = 0;

            string blockHead = "";
            string blockContent = "";

            foreach (var item in code)
            {
                switch (item)
                {
                    case '{':

                        DebugLayer.Assert(inBlock is true, ErrorType.InvaildScriptFormat, currentLine, script.FilePath);

                        inBlock = true;
                        blockStartLine = currentLine;

                        continue;
                    case '}':

                        DebugLayer.Assert(inBlock is false, ErrorType.InvaildScriptFormat, currentLine, script.FilePath);

                        var type = Utilities.StringToScriptType(blockHead);

                        scripts[(int)type].Add(CreateScript(type, blockContent, blockStartLine, script.FilePath));

                        inBlock = false;
                        inString = false;

                        blockContent = "";
                        blockHead = "";

                        continue;
                    case '\n':

                        currentLine++;

                        blockContent += '\n';

                        continue;
                    case '"':

                        if (inBlock is true)
                            inString ^= true;

                        break;
                    default:
                        break;
                }

                if (Utilities.IsEscape(item) is true)
                    continue;

                //Not in block
                if (inBlock is false)
                {
                    if (item == ' ')
                        blockHead = "";
                    else blockHead += item;
                }
                else blockContent += item;

            }
        }

        internal static void Initialize()
        {
            for (int i = 0; i < scripts.Length; i++)
                scripts[i] = new List<Script>();

            foreach (var item in FileManager.Scripts)
            {
                ProcessScriptFile(item);
            }
        }
    }
}
