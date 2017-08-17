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
        private bool isStream = false;

        private SharpDX.XAudio2.SourceVoice sourceVoice;

        private SharpDX.XAudio2.AudioBuffer audioBuffer;
    
        private SharpDX.Multimedia.SoundStream soundStream;

        public VoicePlayer(VoiceBuffer voiceBuffer)
        {
            soundStream = voiceBuffer.SoundStream;

            audioBuffer = new SharpDX.XAudio2.AudioBuffer()
            {
                Stream = voiceBuffer.DataStream,
                AudioBytes = (int)voiceBuffer.SoundStream.Length,
                Flags = SharpDX.XAudio2.BufferFlags.EndOfStream
            };

            sourceVoice = new SharpDX.XAudio2.SourceVoice(Engine.XAudio, voiceBuffer.WaveFormat);
        }

        public VoicePlayer(string fileName)
        {
            soundStream = new SharpDX.Multimedia.SoundStream(System.IO.File.OpenRead(fileName));

            audioBuffer = new SharpDX.XAudio2.AudioBuffer();

            sourceVoice = new SharpDX.XAudio2.SourceVoice(Engine.XAudio, soundStream.Format);
            
            isStream = true;
        }

        public void Play()
        {
            PlayLoop(1);
        }

        public void PlayLoop(int LoopCount)
        {
            isStoping = false;

            if (isStream is true)
            {
                //Need Update
                if (isStoping is true) return;
            }
            else
            {
                audioBuffer.LoopCount = LoopCount;
                sourceVoice.SubmitSourceBuffer(audioBuffer, soundStream.DecodedPacketsInfo);
                sourceVoice.Start();
            }
        }

        public void Stop()
        {
            isStoping = true;

            if (isStream is false)
                sourceVoice.Stop();
        }

        public override void Dispose()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();

            if (isStream is true)
            {
                soundStream.Close();
                SharpDX.Utilities.Dispose(ref soundStream);
            }

            base.Dispose();
        }

        ~VoicePlayer()
        {
            sourceVoice.DestroyVoice();
            sourceVoice.Dispose();

            if (isStream is true)
            {
                soundStream.Close();
                SharpDX.Utilities.Dispose(ref soundStream);
            }
        }

    }
}
