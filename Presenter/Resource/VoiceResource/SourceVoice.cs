using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class SourceVoice : VoiceResource
    {
        private SharpDX.XAudio2.SourceVoice sourceVoice;
        private SharpDX.XAudio2.AudioBuffer audioBuffer;

        private SharpDX.Multimedia.WaveFormat waveFormat;
        private SharpDX.Multimedia.SoundStream soundStream;

        public SourceVoice(string fileName)
        {
            soundStream = new SharpDX.Multimedia.SoundStream(System.IO.File.OpenRead(fileName));

            waveFormat = soundStream.Format;

            audioBuffer = new SharpDX.XAudio2.AudioBuffer()
            {
                Stream = soundStream.ToDataStream(),
                AudioBytes = (int)soundStream.Length,
                Flags = SharpDX.XAudio2.BufferFlags.EndOfStream
            };

            soundStream.Close();

            sourceVoice = new SharpDX.XAudio2.SourceVoice(Engine.XAudio, waveFormat, true);
        }

        public void Play()
        {
            PlayLoop(1);
        }

        public void PlayLoop(int LoopCount)
        {
            audioBuffer.LoopCount = LoopCount;
            sourceVoice.SubmitSourceBuffer(audioBuffer, soundStream.DecodedPacketsInfo);
            sourceVoice.Start();
        }

        public void Stop()
        {
            sourceVoice.Stop();
        }

        ~SourceVoice()
        {
          
        }
    }    

}
