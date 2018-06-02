using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class AnimationList
    {
        private static Dictionary<string, Animation> animationList = new Dictionary<string, Animation>();

        internal static void Add(Animation animation)
        {
            animationList.Add(animation.Name, animation);
        }

        internal static void Remove(Animation animation)
        {
            animationList.Remove(animation.Name);
        }

        public static Animation GetAnimation(string name)
        {
            return animationList[name];
        }
    }
}
