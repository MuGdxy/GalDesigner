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

        private string name = null;

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

        public float EndTime => frames[frames.Count - 1].TimePos;

        public string Name => name;
    }
}
