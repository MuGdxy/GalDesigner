using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Timer
    {
        private float passTime;

        public float PassTime => passTime;

        public Timer(float startTime = 0)
        {
            passTime = startTime;
        }

        public void Reset(float startTime = 0)
        {
            passTime = startTime;
        }

        public void Pass(float PassTime)
        {
            passTime += PassTime;
        }
    }
}
