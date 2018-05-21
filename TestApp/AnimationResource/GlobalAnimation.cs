using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestApp
{
    static class GlobalAnimation
    {
        private static Animation showAnimation;
        private static Animation hideAnimation;

        static GlobalAnimation()
        {
            List<KeyFrame> showFrames = new List<KeyFrame>();

            showFrames.Add(new KeyFrame(0, 0));
            showFrames.Add(new KeyFrame(1, 1));

            List<KeyFrame> hideFrames = new List<KeyFrame>();

            hideFrames.Add(new KeyFrame(1, 0));
            hideFrames.Add(new KeyFrame(0, 1));

            showAnimation = new LinearAnimation(showFrames, "ShowAnimation");
            hideAnimation = new LinearAnimation(hideFrames, "HideAnimation");
        }

        public static Animation ShowAnimation => showAnimation;
        public static Animation HideAnimation => hideAnimation;
    }
}
