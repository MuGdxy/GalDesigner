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
        public static VoiceBuffer voiceBuffer;
        public static VoicePlayer voicePlayer;

        public static void Create()
        {
            //voiceBuffer =  new VoiceBuffer(@"D:\Resource\Test1.wav");
            //voicePlayer = new VoicePlayer(voiceBuffer);
        }

        public static void Destory()
        {
            //voicePlayer.Dispose();
            //voiceBuffer.Dispose();
        }

    }
}
