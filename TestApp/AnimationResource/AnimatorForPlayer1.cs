using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestApp
{
    public class AnimatorForPlayer1 : Animator
    {
        private Animation joinAnimation;
        private Animation leaveAnimation;
        private Animation rotateAnimation;
        private Animation rotateLeaveAnimation;
        private Animation textAnimation;
        private Animation textShowAnimation;

        public AnimatorForPlayer1(string Name) : base(Name)
        {
            float moveLength = 400;

            List<KeyFrame> joinFrames = new List<KeyFrame>();

            joinFrames.Add(new KeyFrame(1920, 0));
            joinFrames.Add(new KeyFrame(1920 - moveLength, 10));

            List<KeyFrame> rotateFrames = new List<KeyFrame>();

            rotateFrames.Add(new KeyFrame(0, 0));
            rotateFrames.Add(new KeyFrame((float)-Math.PI * 2, 10));

            List<KeyFrame> textFrames = new List<KeyFrame>();

            textFrames.Add(new KeyFrame("等等！！😡", 10));
            textFrames.Add(new KeyFrame("新娘，老王我来了！！😘", 15));

            textFrames.Add(new KeyFrame("在浩瀚无边宇宙里 独一无二的一颗", 25));
            textFrames.Add(new KeyFrame("在蔚蓝色的地球上 广袤无垠世界中", 30));
            textFrames.Add(new KeyFrame("将小小的爱恋传达到", 35));
            textFrames.Add(new KeyFrame("小小岛屿上的你身边", 39));
            textFrames.Add(new KeyFrame("与你邂逅 时光流转", 45));
            textFrames.Add(new KeyFrame("填满思念的信笺 与日俱增", 50));
            textFrames.Add(new KeyFrame("不知不觉间 两人心有灵犀", 55));
            textFrames.Add(new KeyFrame("时而热情 时而苦闷", 59));
            textFrames.Add(new KeyFrame("声音渐远 向着遥远的彼方", 64));
            textFrames.Add(new KeyFrame("温柔的歌谣 影响着这世界", 69));
            textFrames.Add(new KeyFrame("看呀 对你而言", 77));
            textFrames.Add(new KeyFrame("最珍视的人会立即赶往你身边", 80));
            textFrames.Add(new KeyFrame("只想为你一人", 86));
            textFrames.Add(new KeyFrame("唱响这回荡着爱恋的歌曲", 90));
            textFrames.Add(new KeyFrame("听呀 听呀 听呀 不断回响着的恋歌", 95));
            textFrames.Add(new KeyFrame("不经意间你发现 两人正并肩而行着", 113));
            textFrames.Add(new KeyFrame("即便昏暗的道路 夜夜也有月光映照", 117));
            textFrames.Add(new KeyFrame("紧握着的手 不会放开", 121));
            textFrames.Add(new KeyFrame("思恋强烈 誓言永恒", 126));
            textFrames.Add(new KeyFrame("即便亘古深渊下 我也一定会", 131));
            textFrames.Add(new KeyFrame("不改初心 说出同样的话语", 135));
            textFrames.Add(new KeyFrame("即便如此还不够 也要让你的泪水", 140));
            textFrames.Add(new KeyFrame("转化成喜悦 言语已经无法细述", 145));
            textFrames.Add(new KeyFrame("仅仅想要拥抱你 仅仅想要拥抱你", 149));

            List<KeyFrame> textShowFrames = new List<KeyFrame>();

            textShowFrames.Add(new KeyFrame(0, 9));
            textShowFrames.Add(new KeyFrame(1, 10));
            textShowFrames.Add(new KeyFrame(1, 149));
            textShowFrames.Add(new KeyFrame(0, 157));
           

            List<KeyFrame> leaveFrames = new List<KeyFrame>();

            leaveFrames.Add(new KeyFrame(1920 - moveLength, 0));
            leaveFrames.Add(new KeyFrame(1920, 10));

            List<KeyFrame> rotateLeaveFrames = new List<KeyFrame>();

            rotateLeaveFrames.Add(new KeyFrame(0, 0));
            rotateLeaveFrames.Add(new KeyFrame((float)Math.PI * 2, 10));

            textAnimation = new Animation(textFrames, "Player1TextAnimation");
            textShowAnimation = new LinearAnimation(textShowFrames, "Player1TextShowAnimation");
            joinAnimation = new LinearAnimation(joinFrames, "Player1JoinAnimation");
            leaveAnimation = new LinearAnimation(leaveFrames, "Player1LeaveAnimation");
            rotateAnimation = new LinearAnimation(rotateFrames, "Player1RotateAnimation");
            rotateLeaveAnimation = new LinearAnimation(rotateLeaveFrames, "Player1RotateLeaveAnimation");

            Add("Player1SayingText", SystemProperty.Text, textAnimation, 105);
            Add("Player1SayingBox", SystemProperty.Opacity, textShowAnimation, 105);

            Add("Player1", SystemProperty.PositionX, joinAnimation, 105);
            Add("Player1", SystemProperty.PositionX, leaveAnimation, 105 + 157);
            Add("Player1SayingBox", SystemProperty.PositionX, joinAnimation, 105);
            Add("Player1", SystemProperty.Angle, rotateAnimation, 105);
            Add("Player1", SystemProperty.Angle, rotateLeaveAnimation, 105 + 157);

            Speed = Program.Speed;
        }
    }
}
