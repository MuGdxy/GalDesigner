using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class VisualGuiComponent : GuiComponent
    {
        public VisualGuiComponent()
        {
            //visual gui component is used for rendering or any visual requirement
            //if you do not have any visual requirement, you can set this visual gui component to object
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(VisualGuiComponent);
        }
    }
}
