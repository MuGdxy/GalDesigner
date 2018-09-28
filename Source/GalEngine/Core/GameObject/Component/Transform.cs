using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public class Transform : Component
    {
        private Matrix3x2 matrix = Matrix3x2.Identity;
        
        public Transform()
        {
            baseComponentType = typeof(Transform);
        }

        public Matrix3x2 Matrix { get => matrix; }
    }
}
