using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    static class ResourcePool
    {
        private static Dictionary<string, CanvasImage> images = new Dictionary<string, CanvasImage>();
        private static Dictionary<string, CanvasBrush> brushs = new Dictionary<string, CanvasBrush>();
        private static Dictionary<string, CanvasTextFormat> textFormats = new Dictionary<string, CanvasTextFormat>();
        private static Dictionary<string, VoiceBuffer> voices = new Dictionary<string, VoiceBuffer>();

        public static void SetResourceTo(ref ImageView imageView)
        {
            var path = imageView.FilePath;

            if (images.ContainsKey(path) is false)
            {
                System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open);

                var image = new CanvasImage(fileStream);

                fileStream.Close();

                images.Add(path, image);

                imageView.Resource = image;

                return;
            }

            imageView.Resource = images[path];
        }

        public static void SetResourceTo(ref BrushView brushView)
        {
            var name = brushView.Name;

            if (brushs.ContainsKey(name) is false)
            {
                var color = brushView.Color;
                var brush = new CanvasBrush(color.X, color.Y, color.Z, color.W);

                brushs.Add(name, brush);

                brushView.Resource = brush;

                return;
            }

            brushView.Resource = brushs[name];
        }

        public static void SetResourceTo(ref VoiceView voiceView)
        {
            var path = voiceView.FilePath;

            if (voices.ContainsKey(path) is false)
            {
                var voice = new VoiceBuffer(path);

                voices.Add(path, voice);

                voiceView.Resource = voice;

                return;
            }

            voiceView.Resource = voices[path];
        }

        public static void SetResourceTo(ref TextFormatView textFormatView)
        {
            var name = textFormatView.Name;

            if (textFormats.ContainsKey(name) is false)
            {
                var textFormat = new CanvasTextFormat(textFormatView.Fontface,
                    textFormatView.Size, textFormatView.Weight);

                textFormats.Add(name, textFormat);

                textFormatView.Resource = textFormat;

                return;
            }

            textFormatView.Resource = textFormats[name];
        }
    }
}
