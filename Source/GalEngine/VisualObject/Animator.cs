using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    public class Animator : IDisposable
    {
        private class AnimationItem : IComparable
        {
            private VisualObject target;
            private string targetMember;

            private Animation animation;

            private float startTime;

            private int preFrame;
            private int nextFrame;

            public AnimationItem(VisualObject Target, string TargetMember, Animation Animation, float StartTime)
            {
                target = Target;
                animation = Animation;
                startTime = StartTime;
                targetMember = TargetMember;

                preFrame = 0;
                nextFrame = 1;
            }

            public VisualObject Target { get => target; }
            public Animation Animation { get => animation;}
            public float StartTime { get => startTime; }

            public void ProcessAnimation(float timePos)
            {
                if (timePos > animation.EndTime) return;

                while (animation.Frames[nextFrame].TimePos <= timePos)
                {
                    preFrame = nextFrame;

                    nextFrame++;

                    if (nextFrame == animation.Frames.Count)
                    {
                        Reset(); return;
                    }
                }

                var frame = animation.GetFrame(timePos, animation.Frames[preFrame],
                    animation.Frames[nextFrame]);

                target.SetMemberValue(targetMember, frame.Value);
            }

            public void Reset()
            {
                preFrame = 0;
                nextFrame = 1;
            }

            public int CompareTo(object obj)
            {
                return startTime.CompareTo((obj as AnimationItem).startTime);
            }
        }

        private string name;

        private bool isRun = false;
        private bool isSorted = false;
        private bool isRepeat = false;

        private float endTime = 0;

        private Timer timer = new Timer();

        private List<AnimationItem> animationItems = new List<AnimationItem>();

        public string Name { get => name; }

        public bool IsSorted { get => isSorted; }

        public bool IsRun { get => isRun; }

        public bool IsRepeat { get => isRepeat; set => isRepeat = value; }

        internal void Update(float passTime)
        {
            if (isRun is false) return;

            timer.Pass(passTime);

            if (timer.PassTime > endTime)
            {
                if (isRepeat is true)
                    timer.Reset(timer.PassTime - endTime);
                else
                {
                    Stop(); return;
                }
            }

            foreach (var item in animationItems)
            {
                if (item.StartTime > timer.PassTime) break;

                if (timer.PassTime > item.Animation.EndTime + item.StartTime)
                    continue;

                item.ProcessAnimation(timer.PassTime - item.StartTime);
            }
        }

        public Animator(string Name)
        {
            name = Name;

            AnimatorList.Add(this);
        }

        public void Add(string targetObject, string targetMember, Animation animation, float startTime)
        {
            DebugLayer.Assert(isRun, ErrorType.CanNotAddAnimationWhenAnimatorRun, animation.Name,
                name);

            var target = VisualObjectList.GetVisualObject(targetObject);

            animationItems.Add(new AnimationItem(target, targetMember,
                animation, startTime));

            endTime = Math.Max(endTime, startTime + animation.EndTime);

            isSorted = false;
        }

        public void Sort()
        {
            isSorted = true;

            animationItems.Sort();
        }

        public void Run(float startTime = 0)
        {
            isRun = true;

            if (isSorted is false)
                Sort();

            timer.Reset(startTime);
        }

        public void Stop()
        {
            isRun = false;
        }

        public void Dispose()
        {
            AnimatorList.Remove(this);
        }
    }
}
