using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Numerics;

namespace GalEngine
{
    public class TransformComponent : Component
    {
        public Vector3 Position { get; set; }
        public Vector2 Scale { get; set; }
        public float Angle { get; set; }

        public TransformComponent()
        {
            BaseComponentType = typeof(TransformComponent);
        }
    }
}
