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
            public string Tag = null;
            public string Fontface = null;
            public float Size = 0;
            public int Weight = 0;
            public Vector4 Color = Vector4.Zero;

            public override string ToString()
            {
                string result = "";

                result += "Type = " + Type;
                result += ", Tag = " + Tag;

                switch (Type)
                {
                    case ResourceType.Unknown:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        break;

                    case ResourceType.Image:
                    case ResourceType.Audio:
                        result += ", FilePath = " + FilePath;
                        break;

                    case ResourceType.TextFormat:
                        result += ", Fontface = " + Fontface;
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
        }

        private Dictionary<string, ResourceTag> resourceList;

        private static void BuildSentenceFromFile(string[] contents, out List<Sentence> sentences)
        {
            sentences = new List<Sentence>();
        }

        private static void BuildSentenceFromList(Dictionary<string, ResourceTag> resourceList, out List<Sentence> sentences)
        {
            sentences = new List<Sentence>();

            foreach (var item in resourceList)
            {
                Sentence sentence = new Sentence
                {
                    Tag = item.Key
                };

                switch (item.Value)
                {
                    case ImageTag imageTag:
                        sentence.Type = ResourceType.Image;
                        sentence.FilePath = imageTag.FilePath;
                        break;

                    case AudioTag audioTag:
                        sentence.Type = ResourceType.Audio;
                        sentence.FilePath = audioTag.FilePath;
                        break;

                    case TextFormatTag textFormatTag:
                        sentence.Type = ResourceType.TextFormat;
                        sentence.Fontface = textFormatTag.Fontface;
                        sentence.Size = textFormatTag.Size;
                        sentence.Weight = textFormatTag.Weight;
                        break;

                    case BrushTag brushTag:
                        sentence.Type = ResourceType.Brush;
                        sentence.Color = brushTag.Color;
                        break;

                    default:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        break;
                }

                sentences.Add(sentence);
            }
        }

        internal ResListAnalyser(string Tag, string FilePath) : base(Tag, FilePath)
        {
            resourceList = new Dictionary<string, ResourceTag>();
        }

        protected override void ProcessReadFile(ref string[] contents)
        {
            BuildSentenceFromFile(contents, out List<Sentence> sentences);

            foreach (var item in sentences)
            {
                switch (item.Type)
                {
                    case ResourceType.Unknown:
                        DebugLayer.ReportError(ErrorType.UnknownResourceType);
                        break;

                    case ResourceType.Image:
                        resourceList.Add(item.Tag, new ImageTag(item.Tag, item.FilePath));
                        break;

                    case ResourceType.Audio:
                        resourceList.Add(item.Tag, new AudioTag(item.Tag, item.FilePath));
                        break;

                    case ResourceType.TextFormat:
                        resourceList.Add(item.Tag, new TextFormatTag(item.Tag, item.Fontface, item.Size, item.Weight));
                        break;

                    case ResourceType.Brush:
                        resourceList.Add(item.Tag, new BrushTag(item.Tag, item.Color));
                        break;

                    default:
                        break;
                }
            }

        }

        protected override void ProcessWriteFile(out string[] contents)
        {
            BuildSentenceFromList(resourceList, out List<Sentence> sentences);

            contents = new string[sentences.Count * 2];

            for (int i = 0; i < sentences.Count; i++)
            {
                contents[i * 2] = sentences[i].ToString();
            }
        }
    }
}
