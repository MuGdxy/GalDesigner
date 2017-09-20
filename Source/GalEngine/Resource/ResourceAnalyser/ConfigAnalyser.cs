﻿using System;
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
            Unknown,
            Int,
            Bool,
            Float,
            String
        }

        private class Sentence
        {
            public ValueType Type = ValueType.Unknown;
            public object Value = null;
            public string ValueName = null;

            public static string Include(string input) => '"' + input + '"';

            public static string UnInclude(string input) => input.Substring(1, input.Length - 2);

            public static ValueType GetType(string value)
            {
                //Test String
                if (value[0] is '"' && value[value.Length - 1] is '"') return ValueType.String;

                //Test Bool
                if (value is "true" || value is "false") return ValueType.Bool;

                int PointCount = 0;

                //Test int and float
                foreach (var item in value)
                {
                    if (item is '.')
                    {
                        PointCount++;

                        if (PointCount > 1) DebugLayer.ReportError(ErrorType.InconsistentResourceParameters, value);

                        continue;
                    }

                    if (item < '0' || item > '9') DebugLayer.ReportError(ErrorType.InconsistentResourceParameters, value);
                }

                if (value.Contains('.')) return ValueType.Float;
                else return ValueType.Int;
            }
        }

        private void ProcessSentenceValue(ref string value)
        {
            Sentence sentence = new Sentence();

            var result = value.Split(new char[] { '=' }, 2);

            var left = result[0]; var right = result[1];

            sentence.ValueName = left;
            sentence.Type = Sentence.GetType(right);

            switch (sentence.Type)
            {
                case ValueType.Int:
                    sentence.Value = Convert.ToInt32(right);
                    break;
                case ValueType.Bool:
                    if (right is "true") sentence.Value = true;
                    else sentence.Value = false;
                    break;
                case ValueType.Float:
                    sentence.Value = (float)Convert.ToDouble(right);
                    break;
                case ValueType.String:
                    sentence.Value = Sentence.UnInclude(right);
                    break;
                default:
                    break;
            }

            GlobalConfig.SetValue(sentence.ValueName, sentence.Value);

            value = "";
        }

        internal ConfigAnalyser(string Tag, string FilePath) : base(Tag, FilePath)
        {
            
        }

        protected override void ProcessReadFile(ref string[] contents)
        {
            string currentString = "";

            bool inCodeBlock = false;
            bool inString = false;

            int line = 0;
            

            foreach (var item in contents)
            {
                line++;

                for (int i = 0; i < item.Length; i++)
                {
                    if (item[i] is '{')
                    {
#if DEBUG
                        DebugLayer.Assert(inCodeBlock is true, ErrorType.InvalidResourceFormat, line, FilePath);
#endif

                        inCodeBlock = true;
                        continue;
                    }
                    
                    if (item[i] is '}')
                    {
#if DEBUG
                        DebugLayer.Assert(inCodeBlock is false, ErrorType.InvalidResourceFormat, line, FilePath);
#endif

                        inCodeBlock = false;

                        ProcessSentenceValue(ref currentString); continue;
                    }

                    if (item[i] is ',' && inString is false)
                    {
                        ProcessSentenceValue(ref currentString); continue;
                    }

                    if (item[i] != ' ' || inString is true)
                        currentString += item[i];

                    if (item[i] is '"') { inString ^= true; continue; }
                }
            }

            if (inCodeBlock is true || inString is true)
                DebugLayer.ReportError(ErrorType.InvalidResourceFormat, line, FilePath);
        }

        protected override void ProcessWriteFile(out string[] contents)
        {
            contents = new string[10];
        }
    }
}
