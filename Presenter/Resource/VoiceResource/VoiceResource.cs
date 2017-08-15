using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VoiceResource : IDisposable
    {
        private static int voiceCount = 0;
  
        private static void CheckVoiceEngine()
        {
            if (voiceCount is 1)
                Engine.CreateAudio();
            else if (voiceCount is 0)
                Engine.DestoryAudio();
        }

        protected VoiceResource()
        {
            voiceCount++;
            CheckVoiceEngine();
        }

        public virtual void Dispose()
        {
            voiceCount--;
            CheckVoiceEngine();
            GC.SuppressFinalize(this);
        }

        ~VoiceResource()
        {
            voiceCount--;
            CheckVoiceEngine();
        }

        
    }
}
