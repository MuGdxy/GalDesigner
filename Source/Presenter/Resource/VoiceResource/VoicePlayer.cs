using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VoicePlayer : VoiceResource
    {
        private bool isStoping = true;

        private SharpDX.XAudio2.SourceVoice sourceVoice;

        private SharpDX.XAudio2.AudioBuffer audioBuffer;
    
        private SharpDX.Multimedia.SoundStream soundStream;

        public VoicePlayer(VoiceBuffer voiceBuffer)
        {
            soundStream = voiceBuffer.SoundStream;

            audioBuffer = new SharpDX.XAudio2.AudioBuffer()
            {
                Stream = voiceBuffer.DataStream,
                AudioBytes = (int)voiceBuffer.DataStream.Length
            };

            sourceVoice = new SharpDX.XAudio2.SourceVoice(Engine.XAudio, voiceBuffer.WaveFormat);
        }

        public void Play()
        {
            PlayLoop(1);
        }

        public void PlayLoop(int LoopCount)
        {
            if (isStoping is false) return;

            isStoping = false;

            sourceVoice.FlushSourceBuffers();

            audioBuffer.LoopCount = LoopCount;
            sourceVoice.SubmitSourceBuffer(audioBuffer, soundStream.DecodedPacketsInfo);
            sourceVoice.Start();
        }

        public void Continue()
        {
            if (isStoping is false) return;

            isStoping = false;
            sourceVoice.Start();
        }

        public void Stop()
        {
            if (isStoping is true) return;

            isStoping = true;
            sourceVoice.Stop();
        }

        public override void Dispose()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();

            base.Dispose();
        }

        public bool IsStoping => isStoping;

        ~VoicePlayer()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();
        }

    }
}
