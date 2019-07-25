using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestUnit
{

    class NewGuiButton : GuiButton
    {
        private bool isDown = false;
        private bool lastMouseStatus = false;

        protected override void Input(InputAction action)
        {
            base.Input(action);
        }

        protected override void Update(float delta)
        {
            if (InputStatus.GetButton(InputProperty.LeftButton) == true
                && Contain(Gui.Position) == false)
            {
                isDown = true;
            }

            if (InputStatus.GetButton(InputProperty.LeftButton) == false
                && lastMouseStatus == true
                && Contain(Gui.Position)
                && isDown == true)
            {
                Console.WriteLine("Up");
            }

            if (InputStatus.GetButton(InputProperty.LeftButton) == false)
                isDown = false;

            lastMouseStatus = InputStatus.GetButton(InputProperty.LeftButton);

            base.Update(delta);
        }

        public NewGuiButton(string content, int fontSize, Size size) : base(content, fontSize, size)
        {
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            GameSystems.Initialize(new GameStartInfo
            {
                Window = new WindowInfo()
                {
                    Name = "TestUnit",
                    Size = new Size(1920, 1080)
                }
            });

            GuiGroup group = new GuiGroup("group0");

            Transform transform = new Transform();

            transform.ApplyTransform(System.Numerics.Matrix4x4.CreateFromAxisAngle(new System.Numerics.Vector3(0, 0, 1), 1));
            transform.ApplyTransform(System.Numerics.Matrix4x4.CreateTranslation(100, 100, 0));

            group.Elements.Add(new NewGuiButton("button", 24, new Size(100, 30))
            {
                Transform = transform,
                Dragable = false
            });

            Gui.Add(group);

            GameSystems.RunLoop();
        }
    }
}
