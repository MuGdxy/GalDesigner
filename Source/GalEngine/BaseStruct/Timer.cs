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
        private float lastPassTime;

        public float PassTime => passTime;
        public float LastPassTime => lastPassTime;

        public Timer(float startTime = 0)
        {
            lastPassTime = 0;
            passTime = startTime;
        }

        public void Reset(float startTime = 0)
        {
            lastPassTime = 0;
            passTime = startTime;
        }

        public void Pass(float PassTime)
        {
            lastPassTime = passTime;
            passTime += PassTime;
        }
    }
}
