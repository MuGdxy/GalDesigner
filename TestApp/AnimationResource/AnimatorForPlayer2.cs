using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestApp
{
    public class AnimatorForPlayer2 : Animator
    {
        private Animation joinAnimation;
        private Animation rotateAnimation;
        private Animation textAnimation;
        private Animation textShowAnimation;
        private Animation powerAnimation;
        private Animation power2Animation;
        private Animation power3Animation;

        public AnimatorForPlayer2(string Name) : base(Name)
        {
            float moveLength = 1920 / 2 - 100;

            List<KeyFrame> joinFrames = new List<KeyFrame>();

            joinFrames.Add(new KeyFrame(-200, 0));
            joinFrames.Add(new KeyFrame(-200 + moveLength, 10));

            List<KeyFrame> rotateFrames = new List<KeyFrame>();

            rotateFrames.Add(new KeyFrame(0, 0));
            rotateFrames.Add(new KeyFrame((float)Math.PI * 4, 10));

            List<KeyFrame> textFrames = new List<KeyFrame>();

            textFrames.Add(new KeyFrame("心想：心情根本无法平静😭。", 10));
            textFrames.Add(new KeyFrame("心想：老师怎么能在婚礼上说这样的话呢？", 60));

            List<KeyFrame> textShowFrames = new List<KeyFrame>();

            textShowFrames.Add(new KeyFrame(0, 10));
            textShowFrames.Add(new KeyFrame(1, 11));
            textShowFrames.Add(new KeyFrame(1, 15));
            textShowFrames.Add(new KeyFrame(0, 16));
            textShowFrames.Add(new KeyFrame(0, 60));
            textShowFrames.Add(new KeyFrame(1, 61));
            textShowFrames.Add(new KeyFrame(1, 70));
            textShowFrames.Add(new KeyFrame(0, 71));

            List<KeyFrame> powerFrames = new List<KeyFrame>();

            powerFrames.Add(new KeyFrame(200, 0));
            powerFrames.Add(new KeyFrame(100, 157));

            List<KeyFrame> power2Frames = new List<KeyFrame>();

            power2Frames.Add(new KeyFrame(440, 0));
            power2Frames.Add(new KeyFrame(490, 157));

            List<KeyFrame> power3Frames = new List<KeyFrame>();

            power3Frames.Add(new KeyFrame(200, 0));
            power3Frames.Add(new KeyFrame(400, 157));

            powerAnimation = new LinearAnimation(powerFrames, "Player2PowerAnimation");
            power2Animation = new LinearAnimation(power2Frames, "Player2Power2Animation");
            power3Animation = new LinearAnimation(power3Frames, "Player2Power3Animation");
            textAnimation = new Animation(textFrames, "Player2TextAnimation");
            textShowAnimation = new LinearAnimation(textShowFrames, "Player2TextShowAnimation");
            joinAnimation = new LinearAnimation(joinFrames, "Player2JoinAnimation");
            rotateAnimation = new LinearAnimation(rotateFrames, "Player2RotateAnimation");
           
            Add("Player2SayingText", SystemProperty.Text, textAnimation, 0);
            Add("Player2SayingBox", SystemProperty.Opacity, textShowAnimation, 0);

            Add("Player2SayingBox", SystemProperty.PositionX, joinAnimation, 5);
            Add("Player2", SystemProperty.PositionX, joinAnimation, 5);
            Add("Player2", SystemProperty.Angle, rotateAnimation, 5);

            Add("Player2", SystemProperty.Height, powerAnimation, 115);
            Add("Player2", SystemProperty.PositionY, power2Animation, 115);
            Add("Player2", SystemProperty.Width, power3Animation, 115);

            Speed = Program.Speed;
        }
    }
}
