using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestApp
{
    class AnimatorForTextBox : Animator
    {
        private Animation showAnimation;
        private Animation storyAnimation;
        private Animation storyFontAnimation;

        public AnimatorForTextBox(string Name) : base(Name)
        {
            List<KeyFrame> storyFrames = new List<KeyFrame>();

            storyFrames.Add(new KeyFrame("欢迎大家今天来参加这对新人的婚礼！", 0));
            storyFrames.Add(new KeyFrame("接下来有请新人入场！", 5));
            storyFrames.Add(new KeyFrame("我宣布，今天的婚礼正式开始！", 15));
            storyFrames.Add(new KeyFrame("婚礼进行中ing..........", 20));
            storyFrames.Add(new KeyFrame("接下来有请证婚人！", 30));
            storyFrames.Add(new KeyFrame("我来是为了讲几句不中听的话，好让社会上知道这样的恶例不足取法，更不值得鼓励。", 35));
            storyFrames.Add(new KeyFrame("新郎，你这个人性情浮躁，以至于学无所成，做学问不成，做人更是失败，你离婚再娶就是用情不专的证明!", 45));
            storyFrames.Add(new KeyFrame("新娘，你和新郎都是过来人，我希望从今以后你能恪遵妇道，检讨自己的个性和行为，离婚再婚都是你们性格的过失所造成的，希望你们不要一错再错自误误人。",
                60));
            storyFrames.Add(new KeyFrame("不要以自私自利作为行事的准则，不要以荒唐和享乐作为人生追求的目的，不要再把婚姻当作是儿戏，以为高兴可以结婚，不高兴可以离婚，让父母汗颜，让朋友不齿，让社会看笑话!",
                75));
            storyFrames.Add(new KeyFrame("总之，我希望这是你们两个人这一辈子最后一次结婚!这就是我对你们的祝贺!―――我说完了!”", 90));
            storyFrames.Add(new KeyFrame("", 105));

            List<KeyFrame> showFrames = new List<KeyFrame>();

            showFrames.Add(new KeyFrame(0, 0));
            showFrames.Add(new KeyFrame(1, 1));

            foreach (var item in storyFrames)
            {
                if (item.TimePos == 0) continue;

                showFrames.Add(new KeyFrame(1, item.TimePos - 1));
                showFrames.Add(new KeyFrame(0, item.TimePos));
                showFrames.Add(new KeyFrame(1, item.TimePos + 1));
            }

            List<KeyFrame> storyFontFrames = new List<KeyFrame>();

            storyFontFrames.Add(new KeyFrame("FirstFont", 0));
            storyFontFrames.Add(new KeyFrame("Font", 35));
            storyFontFrames.Add(new KeyFrame("FirstFont", 105));

            showAnimation = new LinearAnimation(showFrames, "textShowAnimation");
            storyAnimation = new Animation(storyFrames, "storyAnimation");
            storyFontAnimation = new Animation(storyFontFrames, "storyFontAnimation");


            Add("TextBox", SystemProperty.Opacity, showAnimation, 0);
            Add("TextBox", SystemProperty.Text, storyAnimation, 0);
            Add("TextBox", SystemProperty.TextFormat, storyFontAnimation, 0);

            Speed = Program.Speed;
        }
    }
}
