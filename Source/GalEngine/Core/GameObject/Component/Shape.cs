using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public abstract class Shape : Component
    {
        private float opacity = 1.0f;
        private bool visible = true;

        public Shape()
        {
            baseComponentType = typeof(Shape);
        }

        public float Opacity { get => opacity; set => opacity = value; }
        public bool Visible { get => visible; set => visible = value; }
    }
}
