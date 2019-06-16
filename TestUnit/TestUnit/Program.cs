﻿using System;
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

            group.Elements.Add(new GuiInputText("", new Size(100, 25))
            {
                Transform = new GuiTransform()
                {
                    Position = new Point2f(100, 100)
                }
            });

            Gui.Add(group);

            GameSystems.RunLoop();
        }
    }
}
