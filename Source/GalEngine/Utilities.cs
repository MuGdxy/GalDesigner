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

        public static bool IsEscape(char value)
            => value is '\n' || value is '\t' || value is '\r';

        public static bool IsParenthese(char op)
            => op is '(' || op is ')' || op is '[' || op is ']';

        public static bool IsOpenParenthese(char op)
            => op is '(' || op is '[';

        public static bool IsCloseParenthese(char op)
            => op is ')' || op is ']';

        public static bool IsAlphaOrNumber(char value) => IsAlpha(value) || IsNumber(value);

        public static string GetFileSuffix(string file)
        {
            if (file.Contains('.') is false) return null;

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

        public static ScriptType StringToScriptType(string name)
        {
            try
            {
                var type = Enum.Parse(typeof(ScriptType), name);

                return (ScriptType)type;
            }

            catch (ArgumentException)
            {
                return ScriptType.Unknown;
            }
        }

        public static void Dispose<T>(ref T value) where T : class
        {
            if (value is null) return;

            (value as IDisposable).Dispose();

            value = null;
        }
    }
}
