using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalEngine.Runtime.Graphics;

namespace GalEngine
{
    
    public enum PixelFormat : uint
    {
        /// <summary>
        /// red, blue, green, alpha are all 8bit
        /// </summary>
        RedBlueGreenAlpha8bit = GpuPixelFormat.R8G8B8A8Unknown,
        /// <summary>
        /// alpha is 8bit
        /// </summary>
        Alpha8bit = GpuPixelFormat.A8Unknown
    }
}
