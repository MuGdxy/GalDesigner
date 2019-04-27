using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public enum GuiRenderMode
    {
        WireFrame,
        Solid
    }

    public class VisualGuiComponent : GuiComponent
    {
        public Shape Shape { get; }

        public VisualGuiComponent() : this(new Shape())
        {

        }

        public VisualGuiComponent(Shape shape)
        {
            //visual gui component is used for rendering or any visual requirement
            //if you do not have any visual requirement, you can set this visual gui component to object
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(VisualGuiComponent);

            Shape = shape;
        }
    }
}
