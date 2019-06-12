using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalEngine;

namespace TestUnit
{
    public class GuiRectangle : GuiElement
    {
        public Size Size { get; set; }

        protected override void Draw(GuiRender render)
        {
            render.FillRectangle(new Rectanglef(0, 0, Size.Width, Size.Height),
                new Colorf(1, 1, 0, 1));
        }

        public override bool Contain(Point2f point)
        {
            Point2f newPoint = new Point2f(
                point.X - Transform.Position.X,
                point.Y - Transform.Position.Y);

            if (newPoint.X < 0 || newPoint.X > Size.Width) return false;
            if (newPoint.Y < 0 || newPoint.Y > Size.Height) return false;

            return true;
        }
    }

    public class GuiGroup0 : GuiGroup
    {
        public GuiGroup0(string name) : base(name)
        {
            Elements.Add(new GuiRectangle()
            {
                Size = new Size(100, 100),
                Transform = new GuiTransform()
                {
                    Position = new Point2f(0, 0)
                }
            });
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

            Gui.Add(new GuiGroup0("GuiGroup0"));

            GameSystems.RunLoop();
        }
    }
}
