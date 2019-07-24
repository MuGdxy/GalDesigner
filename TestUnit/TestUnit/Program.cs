using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestUnit
{
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

            group.Elements.Add(new GuiButton("button", 24, new Size(100, 30))
            {
                Transform = transform,
                Dragable = true
            });

            Gui.Add(group);

            GameSystems.RunLoop();
        }
    }
}
