using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class TransformGuiComponent : GuiComponent
    {
        public Position<float> Position { get; set; }

        public TransformGuiComponent()
        {
            //transform gui component is used for rendering or any visual requirement
            //it support the information about position and you must set a transform gui component
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(TransformGuiComponent);

            Position = new Position<float>(0, 0);
        }

        public TransformGuiComponent(Position<float> position)
        {
            //transform gui component is used for rendering or any visual requirement
            //it support the information about position and you must set a transform gui component
            //because gui behavior system requires at least three component to run(visual, logic and transform gui component)
            BaseComponentType = typeof(TransformGuiComponent);

            Position = position;
        }
    }
}
