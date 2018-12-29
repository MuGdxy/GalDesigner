using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiComponent : Component
    {
        public GuiComponent()
        {
            BaseComponentType = typeof(GuiComponent);
        }
    }
}
