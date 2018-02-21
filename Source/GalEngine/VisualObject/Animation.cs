using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class KeyFrame<T> : IComparable
    {
        private float timePos;
        private T value;

        public KeyFrame(T Value, float TimePos)
        {
            value = Value;
            timePos = TimePos;
        }

        public T Value => value;
        public float TimePos => timePos;

        public int CompareTo(object obj)
        {
            return timePos.CompareTo((obj as KeyFrame<T>).timePos);
        }
    }


    public class Animation<T>
    {
        private List<KeyFrame<T>> frames;

        private Type templateType = null;

        private int currentPreFrame = 0; // < TimePos
        private int currentLastFrame = 0; // >=TimePos
        private float currentTimePos = 0;

        private bool isStarting = false;
        private bool isLooping = false;

        private object target = null;

        private string name = null;

        internal void OnAdd(object who)
        {
            DebugLayer.Assert(who != null, ErrorType.AnimationIsAdded, name, target);

            target = who;
        }

        internal void OnRemove()
        {
            target = null;
        }

        internal void LoopCurrentTime()
        {
            while (currentTimePos - EndTime > 0)
                currentTimePos -= EndTime;
        }

        internal void Update(ref T value, float passTime)
        {
            if (isStarting is false) return;

            currentTimePos += passTime;

            if (currentTimePos > EndTime)
            {
                if (isLooping is true)
                    LoopCurrentTime();
                else
                {
                    Stop();
                    return;
                }
            }

            while (currentPreFrame < frames.Count)
            {
                if (currentTimePos > frames[currentPreFrame].TimePos)
                    currentPreFrame++;
                else break;
            }

            currentLastFrame = currentPreFrame + 1;

            value = GetFrame(currentTimePos, frames[currentPreFrame], frames[currentLastFrame]).Value;
        }

        protected virtual KeyFrame<T> GetFrame(float timePos,
            KeyFrame<T> preFrame, KeyFrame<T> lastFrame)
        {
            //int and float, we use the linear.
            switch (templateType.Name)
            {
                case "Int32":
                case "Single":
                    float linearScale = (timePos - preFrame.TimePos) / (lastFrame.TimePos - preFrame.TimePos);
                    float preValue = (float)(preFrame.Value as object);
                    float lastValue = (float)(lastFrame.Value as object);
                    float result = (lastValue - preValue) * linearScale;

                    return new KeyFrame<T>((T)(result as object), timePos);
                default:
                    break;
            }

            float preDistance = timePos - preFrame.TimePos;
            float lastDistance = lastFrame.TimePos - timePos;

            if (preDistance <= lastDistance)
                return preFrame;
            else return lastFrame;
        }

        /// <summary>
        /// Create a Animation.
        /// </summary>
        /// <param name="Frames">Frame data.</param>
        /// <param name="animationName">The animation's name.</param>
        public Animation(List<KeyFrame<T>> Frames, string animationName)
        {
            frames = Frames;
            frames.Sort();

            if (frames[0].TimePos != 0)
                frames.Insert(0, new KeyFrame<T>(default(T), 0));

            templateType = typeof(T);

            name = animationName;
        }

        public void Start(float startTime = 0)
        {
            DebugLayer.Assert(target is null, WarningType.NoTargetOfAnimation, name);

            currentTimePos = startTime;

            isStarting = true;

            //the startTime > EndTime
            if (currentTimePos > EndTime)
            {
                if (isLooping is true)
                    LoopCurrentTime();
                else
                {
                    isStarting = false;
                    return;
                }
            }

            for (int i = 0; i < frames.Count; i++)
            {
                if (frames[i].TimePos >= currentTimePos)
                {
                    currentLastFrame = i;
                    currentPreFrame = i - 1;
                    break;
                }
            }
        }

        public void Continue()
        {
            DebugLayer.Assert(target is null, WarningType.NoTargetOfAnimation, name);

            isStarting = true;
        }

        public void Stop()
        {
            isStarting = false;
        }

        public float EndTime => frames[frames.Count - 1].TimePos;

        public bool IsStarting => IsStarting;

        public bool IsLooping
        {
            set => isLooping = value;
            get => isLooping;
        }
    }
}
