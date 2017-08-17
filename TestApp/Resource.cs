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
        public static VoiceBuffer voiceBuffer1 = new VoiceBuffer(@"D:\Resource\Test1.wav");
        public static VoiceBuffer voiceBuffer2 = new VoiceBuffer(@"D:\Resource\Test2.wav");
        public static VoicePlayer voicePlayer1 = new VoicePlayer(voiceBuffer1);
        public static VoicePlayer voicePlayer2 = new VoicePlayer(voiceBuffer2);

        public static void Destory()
        {
            voiceBuffer1.Dispose();
            voiceBuffer2.Dispose();
            voicePlayer1.Dispose();
            voicePlayer2.Dispose();
        }
    }
}
