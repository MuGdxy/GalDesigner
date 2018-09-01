using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class Commponent
    {
        protected internal abstract void OnRender(GameObject gameObject);
    }
}
