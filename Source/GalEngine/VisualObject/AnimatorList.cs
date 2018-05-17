using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class AnimatorList
    {
        private static Dictionary<string, Animator> animatorList = new Dictionary<string, Animator>();

        internal static void Add(Animator animator)
        {
            animatorList[animator.Name] = animator;
        }

        internal static void Remove(Animator animator)
        {
            animatorList.Remove(animator.Name);
        }

        internal static void Update()
        {
            foreach (var item in animatorList)
            {
                item.Value.Update(Time.DeltaSeconds);
            }
        }

        public static Animator GetAnimation(string name)
        {
            return animatorList[name];
        }
    }
}
