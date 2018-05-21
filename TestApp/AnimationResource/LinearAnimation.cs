using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestApp
{
    public class LinearAnimation : Animation
    {
        protected override KeyFrame GetFrame(float timePos, KeyFrame preFrame, KeyFrame lastFrame)
        {
            float scale = (timePos - preFrame.TimePos) / (lastFrame.TimePos - preFrame.TimePos);

            float preValue = Convert.ToSingle(preFrame.Value);
            float lastValue = Convert.ToSingle(lastFrame.Value);

            return new KeyFrame(preValue + (lastValue - preValue) * scale, timePos);
        }

        public LinearAnimation(List<KeyFrame> Frames, string animationName) : base(Frames, animationName)
        {

        }

        public static LinearAnimation LoadAnimation(string fileName, string animationName)
        {
            var data = System.IO.File.ReadAllLines(fileName);

            var frames = new List<KeyFrame>();

            foreach (var item in data)
            {
                var str = item.Split(' ');

                frames.Add(new KeyFrame(Convert.ToSingle(str[0]), Convert.ToSingle(str[1])));
            }

            return new LinearAnimation(frames, animationName);
        }
    }
}
