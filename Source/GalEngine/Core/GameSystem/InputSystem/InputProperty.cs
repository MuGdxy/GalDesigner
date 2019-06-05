using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class InputProperty
    {
        /// <summary>
        /// Button Input: Left Mouse Button Name
        /// </summary>
        public static string LeftButton => "LeftButton";
        /// <summary>
        /// Button Input: Middle Mouse Button Name
        /// </summary>
        public static string MiddleButton => "MiddleButton";
        /// <summary>
        /// Button Input: Right Mouse Button Name
        /// </summary>
        public static string RightButton => "RightButton";

        /// <summary>
        /// Axis Input: Mouse Wheel Axis Name. Offset from (-inf) to (inf)
        /// </summary>
        public static string MouseWheel => "MouseWheel";

        /// <summary>
        /// Axis Input: Mouse Position X Axis Name. Offset from (-1) to (1).
        /// </summary>
        public static string MouseX => "MouseX";
        /// <summary>
        /// Axis Input: Mouse Position Y Axis Name. Offset from (-1) to (1).
        /// </summary>
        public static string MouseY => "MouseY";
    }
}
