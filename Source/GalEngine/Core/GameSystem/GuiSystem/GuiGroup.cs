using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiGroup : GuiElement
    {
        public List<GuiElement> Elements { get; }

        protected internal override void Draw(GuiRender render) 
            => Elements.ForEach((element) => element.Draw(render));

        protected internal override void Update(float delta)
            => Elements.ForEach((element) => element.Update(delta));

        protected internal override void Input(InputAction action)
        {
            throw new Exception("");
        }

        public override bool Contain(Point2f point)
        {
            foreach (var element in Elements)
                if (element.Contain(point)) return true;
            return false;
        }

        public string Name { get; }

        public GuiGroup(string name)
        {
            Name = name;
            Elements = new List<GuiElement>();
        }
    }
}
