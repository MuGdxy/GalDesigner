using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VoicePlayer : VoiceResource
    {
        private SharpDX.XAudio2.SourceVoice sourceVoice;

        private SharpDX.XAudio2.AudioBuffer audioBuffer;
    
        private VoiceBuffer voiceBuffer;

        public VoicePlayer(VoiceBuffer voiceBuffer)
        {
            this.voiceBuffer = voiceBuffer;

            audioBuffer = new SharpDX.XAudio2.AudioBuffer()
            {
                Stream = voiceBuffer.DataStream,
                AudioBytes = (int)voiceBuffer.SoundStream.Length,
                Flags = SharpDX.XAudio2.BufferFlags.EndOfStream
            };

            sourceVoice = new SharpDX.XAudio2.SourceVoice(Engine.XAudio, voiceBuffer.WaveFormat);
        }

        public void Play()
        {
            PlayLoop(1);
        }

        public void PlayLoop(int LoopCount)
        {
            audioBuffer.LoopCount = LoopCount;
            sourceVoice.SubmitSourceBuffer(audioBuffer, voiceBuffer.SoundStream.DecodedPacketsInfo);
            sourceVoice.Start();
        }

        public void Stop()
        {
            sourceVoice.Stop();
        }

        public override void Dispose()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();

            base.Dispose();
        }

        ~VoicePlayer()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();
        }

    }
}
