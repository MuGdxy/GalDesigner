using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VoiceBuffer : VoiceResource
    {
        private SharpDX.DataStream dataStream;

        private SharpDX.Multimedia.WaveFormat waveFormat;
        private SharpDX.Multimedia.SoundStream soundStream;
        
        public VoiceBuffer(string fileName)
        {
            soundStream = new SharpDX.Multimedia.SoundStream(System.IO.File.OpenRead(fileName));

            waveFormat = soundStream.Format;

            dataStream = soundStream.ToDataStream();
            
            soundStream.Close();
        }

        internal SharpDX.DataStream DataStream => dataStream;

        internal SharpDX.Multimedia.WaveFormat WaveFormat => waveFormat;

        internal SharpDX.Multimedia.SoundStream SoundStream => soundStream;

        public override void Dispose()
        {
            dataStream.Dispose();

            base.Dispose();
        }

        ~VoiceBuffer()
        {
            dataStream.Dispose();
        }
    }
}
