using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class VoiceView : ResourceView
    {
        private VoicePlayer voicePlayer;

        private string filePath;

        public VoiceView(string name, string FilePath) : base(name)
        {
            voicePlayer = null;
            filePath = FilePath;
        }

        public VoicePlayer VoicePlayer => voicePlayer;

        public VoiceBuffer Source => Resource as VoiceBuffer;

        public string FilePath => filePath;

    }
}
