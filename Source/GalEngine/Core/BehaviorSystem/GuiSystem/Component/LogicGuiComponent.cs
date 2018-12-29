using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    public class LogicGuiComponent : GuiComponent
    {
        public LogicGuiComponent()
        {
            //logic gui component is used for logic
            //if you do not have any logic requirement, you can set this logic gui component to object
            //because gui behavior system requires at least two component to run(visual and logic gui component)
            BaseComponentType = typeof(LogicGuiComponent);
        }
    }
}
