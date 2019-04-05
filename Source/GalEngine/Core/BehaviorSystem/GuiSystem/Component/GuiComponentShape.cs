using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class GuiComponentShape
    {
        public virtual bool Contain(Position<float> position)
        {
            return false;
        }
    }
}
