using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class ScriptProcessUnit
    {
        protected struct Sentence
        {
            public string Code;
            public int Line;

            public Sentence(string code,int line)
            {
                Code = code;
                Line = line;
            }
        }

        private static ScriptProcessUnit defaultProcessUnit = new ScriptProcessUnit();

        private static Sentence[] AnalyerScript(Script script)
        {
            var result = new List<Sentence>();

            int line = script.Line;

            bool isString = false;

            string sentence = "";

            foreach (var item in script.Code)
            {
                switch (item)
                {
                    case '\n':
                        line++;

                        continue;
                    case ';':
                        if (isString is false)
                        {
                            result.Add(new Sentence(sentence, line));

                            sentence = "";

                            continue;
                        }

                        break;
                    case ' ':
                        if (isString is false) continue;
                        break;

                    case '"':
                        isString ^= true;
                        break;

                    default:
                        break;
                }

                sentence += item;
            }

            DebugLayer.Assert(sentence != "", ErrorType.InvaildScriptFormat, line, script.FilePath);

            return result.ToArray();
        }

        protected virtual void AnalyerSentence(Script script, Sentence[] sentences)
        {
            foreach (var item in sentences)
            {
                
            }
        }

        public void ProcessScript(Script script)
        {
            var sentences = AnalyerScript(script);

            AnalyerSentence(script, sentences);
        }

        public static ScriptProcessUnit Default => defaultProcessUnit;
    }
}
