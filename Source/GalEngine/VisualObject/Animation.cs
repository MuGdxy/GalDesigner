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

        private FrameProcessUnit frameProcessUnit = AnimationExtension.Default;

        internal List<KeyFrame> Frames => frames;

        public float EndTime
        {
            get
            { 
                return frames[frames.Count - 1].TimePos;
            }
        }

        public string Name => name;

        public FrameProcessUnit FrameProcessUnit
        {
            set => frameProcessUnit = value;
            get => frameProcessUnit;
        }

        public void Dispose()
        {
            AnimationList.Remove(this);
        }

        /// <summary>
        /// Create a Animation.
        /// </summary>
        /// <param name="Frames">Frame data.</param>
        /// <param name="AnimationName">The animation's name.</param>
        public Animation(List<KeyFrame> Frames, string AnimationName)
        {
            frames = Frames;
            frames.Sort();

            DebugLayer.Assert(frames.Count < 2, ErrorType.MoreFramesNeedInAnimation, AnimationName);

            name = AnimationName;

            AnimationList.Add(this);
        }

        public Animation(List<KeyFrame> Frames, string AnimationName, string ProcessUnitName) : this(Frames, AnimationName)
        {
            frameProcessUnit = AnimationExtension.GetProcessUnit(ProcessUnitName);
        }
    }
}
