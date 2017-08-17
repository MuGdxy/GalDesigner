using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    public class VoiceResource : IDisposable
    {
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~VoiceResource()
        {
        }

        
    }
}
