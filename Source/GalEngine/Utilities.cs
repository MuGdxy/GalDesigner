using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class Utilities
    {
        public static bool IsBaseType(object value)
            => value is int || value is bool || value is string || value is float;


    }
}
