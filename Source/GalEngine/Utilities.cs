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

        public static bool IsAlpha(char value)
            => (value >= 'a' && value <= 'z') || (value >= 'A' && value <= 'Z');

        public static bool IsNumber(char value)
            => value >= '0' && value <= '9';

        public static bool IsAlphaOrNumber(char value) => IsAlpha(value) || IsNumber(value);
    }
}
