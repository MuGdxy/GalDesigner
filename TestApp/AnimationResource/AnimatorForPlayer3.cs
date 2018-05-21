using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestApp
{
    public class AnimatorForPlayer3 : Animator
    {
        private Animation joinAnimation;
        private Animation leaveAnimation;
        private Animation rotateAnimation;
        private Animation rotateLeaveAnimation;

        public AnimatorForPlayer3(string Name) : base(Name)
        {
            float moveLength = 1920 / 2 - 100;

            List<KeyFrame> joinFrames = new List<KeyFrame>();

            joinFrames.Add(new KeyFrame(1920, 0));
            joinFrames.Add(new KeyFrame(1920 - moveLength, 10));

            List<KeyFrame> rotateFrames = new List<KeyFrame>();

            rotateFrames.Add(new KeyFrame(0, 0));
            rotateFrames.Add(new KeyFrame((float)-Math.PI * 4, 10));

            List<KeyFrame> leaveFrames = new List<KeyFrame>();

            leaveFrames.Add(new KeyFrame(1920 - moveLength, 0));
            leaveFrames.Add(new KeyFrame(1920, 10));

            List<KeyFrame> rotateLeaveFrames = new List<KeyFrame>();

            rotateLeaveFrames.Add(new KeyFrame(0, 0));
            rotateLeaveFrames.Add(new KeyFrame((float)Math.PI * 4, 10));

            joinAnimation = new LinearAnimation(joinFrames, "Player3JoinAnimation");
            leaveAnimation = new LinearAnimation(leaveFrames, "Player3LeaveAnimation");
            rotateAnimation = new LinearAnimation(rotateFrames, "Player3RotateAnimation");
            rotateLeaveAnimation = new LinearAnimation(rotateLeaveFrames, "Player3RotateLeaveAnimation");

            Add("Player3", SystemProperty.PositionX, joinAnimation, 5);
            Add("Player3", SystemProperty.PositionX, leaveAnimation, 105 + 157);
            Add("Player3", SystemProperty.Angle, rotateAnimation, 5);
            Add("Player3", SystemProperty.Angle, rotateLeaveAnimation, 105 + 157);
           

            Speed = Program.Speed;
        }
    }
}
