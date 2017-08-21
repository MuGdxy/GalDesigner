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

        public VoiceBuffer(VoiceStream voiceStream, int byteStartPos = 0, long byteLength = 0)
        {
#if DEBUG
            if (byteLength + byteStartPos - 1 >= voiceStream.Length)
                throw new ArgumentOutOfRangeException("Length is out of range.");
#endif
            voiceStream.SoundStream.Position = byteStartPos;

            byte[] buffer = new byte[byteLength];

            voiceStream.SoundStream.Read(buffer, 0, (int)byteLength);

            dataStream = SharpDX.DataStream.Create(buffer, true, true);

            waveFormat = voiceStream.SoundStream.Format;
            soundStream = voiceStream.SoundStream;
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
