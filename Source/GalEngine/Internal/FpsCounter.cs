using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine.Internal
{
    internal class FpsCounter
    {
        private int exportCnt = 0;

        private float passTime = 0;

        private int fps = 0;

        private long fpsTime = 0;
        private long count = 1;

        public void OnUpdate()
        {
            passTime += Time.DeltaSeconds;

            if (passTime >= 1.0f)
            {
                fps = exportCnt;

                exportCnt = 0;

                passTime -= 1.0f;

                fpsTime += fps;

                count++;
            }
        }

        public void OnExport()
        {
            exportCnt++;
        }

        public float FpsAverage => fpsTime / (float)count;

        public int Fps => fps;

    }
}
