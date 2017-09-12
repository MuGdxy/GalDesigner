using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace GalEngine
{
    class AudioTag : ResourceTag
    {
        private VoicePlayer voicePlayer;

        private string filePath;

        public AudioTag(string Tag, string FilePath) : base(Tag)
        {
            voicePlayer = null;
            filePath = FilePath;
        }

        protected override void ActiveResource(ref object resource)
        {
            if (resource is null)
            {
                resource = new VoiceBuffer(filePath);
                voicePlayer = new VoicePlayer(resource as VoiceBuffer);
            }
        }

        protected override void DiposeResource(ref object resource)
        {
            if (resource is null) return;

            voicePlayer.Dispose();
            (resource as VoiceBuffer).Dispose();
            voicePlayer = null;
            resource = null;
        }

        public string FilePath => filePath;
        public VoicePlayer VoicePlayer => voicePlayer;
    }
}
