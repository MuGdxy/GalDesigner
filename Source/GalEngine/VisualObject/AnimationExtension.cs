using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public delegate KeyFrame FrameProcessUnit(float timePos, KeyFrame preFrame, KeyFrame nextFrame);

    public static class AnimationExtension
    {
        private static Dictionary<string, FrameProcessUnit> processUnit = new Dictionary<string, FrameProcessUnit>();

        private static KeyFrame DefaultProcessUnit(float timePos, KeyFrame preFrame, KeyFrame nextFrame)
        {
            if (timePos == nextFrame.TimePos) return nextFrame;
            return preFrame;
        }

        static AnimationExtension()
        {
            AddFrameProcessUnit("Default", DefaultProcessUnit);
        }

        public static void AddFrameProcessUnit(string name,FrameProcessUnit frameProcessUnit)
        {
            processUnit[name] = frameProcessUnit;
        }

        public static FrameProcessUnit GetProcessUnit(string name)
        {
            DebugLayer.Assert(processUnit.ContainsKey(name) is false, ErrorType.InvaildFrameProcessUnit,
                name);

            return processUnit[name];
        }

        public static FrameProcessUnit Default => DefaultProcessUnit;
    }
}
