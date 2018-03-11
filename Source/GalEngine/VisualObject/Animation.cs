using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class KeyFrame : IComparable
    {
        private float timePos;
        private object value;

        public KeyFrame(object Value, float TimePos)
        {
            value = Value;
            timePos = TimePos;
        }

        public object Value => value;
        public float TimePos => timePos;

        public int CompareTo(object obj)
        {
            return timePos.CompareTo((obj as KeyFrame).timePos);
        }
    }


    public class Animation : IDisposable
    {
        private List<KeyFrame> frames;
        
        private string name = null;

        protected virtual KeyFrame GetFrame(float timePos,
            KeyFrame preFrame, KeyFrame lastFrame)
        {
            float preDistance = timePos - preFrame.TimePos;
            float lastDistance = lastFrame.TimePos - timePos;

            if (preDistance <= lastDistance)
                return preFrame;
            else return lastFrame;
        }

        public void Dispose()
        {
            AnimationList.Remove(this);
        }

        /// <summary>
        /// Create a Animation.
        /// </summary>
        /// <param name="Frames">Frame data.</param>
        /// <param name="animationName">The animation's name.</param>
        public Animation(List<KeyFrame> Frames, string animationName)
        {
            frames = Frames;
            frames.Sort();

            DebugLayer.Assert(frames.Count is 0, WarningType.NoFramesInAnimation, animationName);

            name = animationName;

            AnimationList.Add(this);
        }

        public float EndTime
        {
            get
            {
                if (frames.Count is 0) return 0;
                else return frames[frames.Count - 1].TimePos;
            }
        }

        public string Name => name;
    }
}
