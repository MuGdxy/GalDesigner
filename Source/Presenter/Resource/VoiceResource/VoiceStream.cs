using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VoiceStream : VoiceResource
    {
        private SharpDX.Multimedia.SoundStream soundStream;

        public VoiceStream(string fileName)
        {
            soundStream = new SharpDX.Multimedia.SoundStream(System.IO.File.OpenRead(fileName));
        }

        public override void Dispose()
        {
            soundStream.Close();

            SharpDX.Utilities.Dispose(ref soundStream);
            base.Dispose();
        }

        internal SharpDX.Multimedia.SoundStream SoundStream => soundStream;

        public long Length => soundStream.Length;


        ~VoiceStream()
        {
            soundStream.Close();

            SharpDX.Utilities.Dispose(ref soundStream);
        }
    }
}
