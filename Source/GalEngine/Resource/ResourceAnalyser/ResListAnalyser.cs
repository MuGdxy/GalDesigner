using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace GalEngine
{
    public class ResListAnalyser : ResourceAnalyser
    {
        private enum ResourceType
        {
            Unknown,
            Image,
            Audio,
            TextFormat,
            Brush
        }

        private class Sentence
        {
            public ResourceType Type = ResourceType.Unknown;
            public string FilePath = null;
            public string Name = null;
            public string Fontface = null;
            public float Size = 0;
            public int Weight = 0;
            public Vector4 Color = Vector4.Zero;

            public override string ToString()
            {
                string result = "";

                result += "Type = " + Type;
                result += ", Name = " + Include(Name);

                switch (Type)
                {
                    case ResourceType.Unknown:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        break;

                    case ResourceType.Image:
                    case ResourceType.Audio:
                        result += ", FilePath = " + Include(FilePath);
                        break;

                    case ResourceType.TextFormat:
                        result += ", Fontface = " + Include(Fontface);
                        result += ", Size = " + Size;
                        result += ", Weight = " + Weight;
                        break;

                    case ResourceType.Brush:
                        result += ", Red = " + Color.X;
                        result += ", Green = " + Color.Y;
                        result += ", Blue = " + Color.Z;
                        result += ", Alpha = " + Color.W;
                        break;

                    default:
                        break;
                }
                return "[" + result + "]";
            }

            public bool IsError()
            {
                if (Name is null) return true;

                switch (Type)
                {
                    case ResourceType.Unknown:
                        return true;
                       
                    case ResourceType.Image:
                        return FilePath is null;

                    case ResourceType.Audio:
                        return FilePath is null;

                    case ResourceType.TextFormat:
                        return Fontface is null | Size is 0 | Weight is 0; 

                    case ResourceType.Brush:
                        return Color == -Vector4.One;

                    default:
                        return true;
                }
            }

            public static string Include(string input) => '"' + input + '"';

            public static string Unclude(string input) => input.Substring(1, input.Length - 2);

            public static ResourceType GetType(string TypeName)
            {
                switch (TypeName)
                {
                    case "Image":
                        return ResourceType.Image;

                    case "Audio":
                        return ResourceType.Audio;

                    case "TextFormat":
                        return ResourceType.TextFormat;

                    case "Brush":
                        return ResourceType.Brush;

                    default:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        return ResourceType.Unknown;
                }
            }
        }

        private Dictionary<string, ResourceView> resourceList;

        private void ProcessSentenceValue(ref Sentence sentence, ref string value, int Line, string FileName)
        {
            var result = value.Split(new char[] { '=' }, 2);

            var left = result[0]; var right = result[1];

            switch (left)
            {
                case "Type":
                    sentence.Type = Sentence.GetType(right);
                    break;

                case "Name":
                    sentence.Name = Sentence.Unclude(right);
                    break;

                case "FilePath":
                    sentence.FilePath = Sentence.Unclude(right);
                    break;

                case "Fontface":
                    sentence.Fontface = Sentence.Unclude(right);
                    break;

                case "Size":
                    sentence.Size = (float)Convert.ToDouble(right);
                    break;

                case "Weight":
                    sentence.Weight = Convert.ToInt32(right);
                    break;

                case "Red":
                    sentence.Color.X = (float)Convert.ToDouble(right);
                    break;

                case "Green":
                    sentence.Color.Y = (float)Convert.ToDouble(right);
                    break;

                case "Blue":
                    sentence.Color.Z = (float)Convert.ToDouble(right);
                    break;

                case "Alpha":
                    sentence.Color.W = (float)Convert.ToDouble(right);
                    break;


                default:
                    DebugLayer.ReportError(ErrorType.InconsistentResourceParameters, Line, FileName);
                    break;
            }

            value = "";
        }

        private void BuildSentenceFromFile(string contents, string FileName, out List<Sentence> sentences)
        {
            sentences = new List<Sentence>();

            Sentence currentSentence = null;

            string currentString = "";
            bool inString = false;


            bool inSentence = false;

            int line = 0;

            foreach (var item in contents)
            {
                if (item is '\n') { line++; continue; }

                //Find String Value's Name
                if (item is '"') { currentString += item; inString ^= true; continue; }

                //Build String for making Sentence
                if (Utilities.IsAlphaOrNumber(item) is true || item is '=' || inString is true)
                {
                    currentString += item;
                    continue;
                }

                if (item is '[')
                {

                    DebugLayer.Assert(inSentence is true, ErrorType.InvalidResourceFormat, line, FileName);
                    
                    inSentence = true;

                    currentSentence = new Sentence(); continue;
                }

                if (item is ']')
                {

                    DebugLayer.Assert(inSentence is false, ErrorType.InvalidResourceFormat, line, FileName);
                    inSentence = false;


                    ProcessSentenceValue(ref currentSentence, ref currentString, line, FileName);


                    DebugLayer.Assert(currentSentence.IsError(), ErrorType.InconsistentResourceParameters,
                        currentSentence.ToString());

                    sentences.Add(currentSentence); continue;
                }

                //Find a value
                if (item is ',')
                {
                    ProcessSentenceValue(ref currentSentence, ref currentString, line, FileName);

                    continue;
                }
            }


            DebugLayer.Assert(inSentence is true | inString is true, ErrorType.InvalidResourceFormat,
                contents.Length, FileName);


        }

        private void BuildSentenceFromList(Dictionary<string, ResourceView> resourceList, out List<Sentence> sentences)
        {
            sentences = new List<Sentence>();

            foreach (var item in resourceList)
            {
                Sentence sentence = new Sentence
                {
                    Name = item.Key
                };

                switch (item.Value)
                {
                    case ImageView imageView:
                        sentence.Type = ResourceType.Image;
                        sentence.FilePath = imageView.FilePath;
                        break;

                    case AudioView audioView:
                        sentence.Type = ResourceType.Audio;
                        sentence.FilePath = audioView.FilePath;
                        break;

                    case TextFormatView textFormatView:
                        sentence.Type = ResourceType.TextFormat;
                        sentence.Fontface = textFormatView.Fontface;
                        sentence.Size = textFormatView.Size;
                        sentence.Weight = textFormatView.Weight;
                        break;

                    case BrushView brushView:
                        sentence.Type = ResourceType.Brush;
                        sentence.Color = brushView.Color;
                        break;

                    default:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        break;
                }

                sentences.Add(sentence);
            }
        }

        internal ResListAnalyser(string Name, string FilePath) : base(Name, FilePath)
        {
            resourceList = new Dictionary<string, ResourceView>();
        }

        protected override void ProcessReadFile(ref string contents)
        {
            BuildSentenceFromFile(contents, FilePath, out List<Sentence> sentences);

            foreach (var item in sentences)
            {
                switch (item.Type)
                {
                    case ResourceType.Unknown:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        break;

                    case ResourceType.Image:
                        resourceList.Add(item.Name, new ImageView(item.Name, item.FilePath));
                        break;

                    case ResourceType.Audio:
                        resourceList.Add(item.Name, new AudioView(item.Name, item.FilePath));
                        break;

                    case ResourceType.TextFormat:
                        resourceList.Add(item.Name, new TextFormatView(item.Name, item.Fontface, item.Size, item.Weight));
                        break;

                    case ResourceType.Brush:
                        resourceList.Add(item.Name, new BrushView(item.Name, item.Color));
                        break;

                    default:
                        break;
                }
            }

        }

        protected override void ProcessWriteFile(out string contents)
        {
            BuildSentenceFromList(resourceList, out List<Sentence> sentences);

            contents = "";

            for (int i = 0; i < sentences.Count; i++)
            {
                contents += sentences[i] + "\n";
            }
        }
    }
}
