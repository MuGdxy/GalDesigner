using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class DebugLayer
    {
        private static Dictionary<ErrorType, string> errorText;

        private static string SetParamsToString(string text, params object[] value)
        {
            string result = text;

            int count = 0;

            while (text.Contains("{" + count + "}") is true)
            {
                result = result.Replace("{" + count + "}", value[count].ToString());

                count++;
            }

            return result;
        }

        static DebugLayer()
        {
            errorText = new Dictionary<ErrorType, string>();
        }

        public static Exception GetErrorException(ErrorType errorType, params object[] value)
        {
            return new Exception(SetParamsToString(errorText[errorType], value));
        }
    }
}
