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

        public static string GetFileSuffix(string file)
        {
            if (file.Contains('.') is false)
                throw new Exception("Get FileSuffix failed, the file does not have suffix.");

            var suffix = "";

            for (int i = file.Length - 1; i >= 0; i--)
            {
                if (file[i] is '.') break;

                suffix += file[i];
            }

            var result = "";

            for (int i = suffix.Length - 1; i >= 0; i--)
                result += suffix[i];

            return result;
        }
    }
}
