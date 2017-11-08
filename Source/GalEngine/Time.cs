using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class Time
    {
        private static DateTime lastTime = DateTime.Now;
        private static DateTime nowTime = DateTime.Now;
        private static TimeSpan deltaTime = nowTime - lastTime;

        internal static void Update()
        {
            lastTime = nowTime;
            nowTime = DateTime.Now;
            deltaTime = nowTime - lastTime;
        }

        public static DateTime NowTime => nowTime;

        public static TimeSpan DeltaTime => deltaTime;

        public static float DeltaSeconds => (float)deltaTime.TotalSeconds;
    }
}
