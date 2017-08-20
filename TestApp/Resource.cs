using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Presenter;

namespace TestApp
{
    public static class Resource
    {
        public static VoiceStream voiceStream1;
        public static VoiceStream voiceStream2;
        public static VoiceBuffer voiceBuffer1;
        public static VoiceBuffer voiceBuffer2;
        public static VoicePlayer voicePlayer1;
        public static VoicePlayer voicePlayer2;

        public static void Create()
        {
            voiceStream1 = new VoiceStream(@"D:\Resource\Test1.wav");
            voiceStream2 = new VoiceStream(@"D:\Resource\Test2.wav");
            voiceBuffer1 = new VoiceBuffer(voiceStream1, 0, 10000000);
            voiceBuffer2 = new VoiceBuffer(voiceStream2, 0, 1000);
            voicePlayer1 = new VoicePlayer(voiceBuffer1);
            voicePlayer2 = new VoicePlayer(voiceBuffer2);
        }

        public static void Destory()
        {
            voiceStream1.Dispose();
            voiceStream2.Dispose();
            voiceBuffer1.Dispose();
            voiceBuffer2.Dispose();
            voicePlayer1.Dispose();
            voicePlayer2.Dispose();
        }

    }
}
