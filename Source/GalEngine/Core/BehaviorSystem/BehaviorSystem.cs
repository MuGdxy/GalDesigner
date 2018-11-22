using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class BehaviorSystem
    {
        public RequireComponents RequireComponents { get; set; }

        public BehaviorSystem()
        {
            RequireComponents = new RequireComponents();
        }

        public abstract void Excute(GameObject gameObject);
        
    }
}
