using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
    [Flags]
    public enum ColorMask
    {
        Red = 1,
        Green = 2,
        Blue = 4,
        Alpha = 8,
        All = 15
    }
}
