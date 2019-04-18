using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    public class LogicGuiComponent : GuiComponent
    {
        public MappedContainer<GuiComponentEventPart> EventParts { get; }

        public LogicGuiComponent()
        {
            //logic gui component is used for logic
            //if you do not have any logic requirement, you can set this logic gui component to object
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(LogicGuiComponent);

            //initialize the event parts container
            EventParts = new MappedContainer<GuiComponentEventPart>();
        }
    }
}
