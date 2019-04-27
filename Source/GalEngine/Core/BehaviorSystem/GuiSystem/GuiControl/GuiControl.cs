using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class GuiControl : GameObject
    {
        public GuiControl() : this(null)
        {

        }

        public GuiControl(string name) : base(name)
        {
            //add requirement component
            AddComponent(new VisualGuiComponent());
            AddComponent(new TransformGuiComponent());
        }
    }
}
