using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class DebugLayer
    {
        private static Dictionary<ErrorType, string> errorText;
        private static Dictionary<WarningType, string> warningText;

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
            warningText = new Dictionary<WarningType, string>();
        }

        public static Exception GetErrorException(ErrorType errorType, params object[] value)
        {
            return new Exception(SetParamsToString(errorText[errorType], value));
        }

        public static void ReportError(ErrorType errorType, params object[] value)
        {
            throw GetErrorException(errorType, value);
        }

        public static void ReportWarning(WarningType warningType, params object[] value)
        {

        }

        public static void Assert(bool testValue, ErrorType errorType, params object[] value)
        {
            if (testValue is true) ReportError(errorType, value);
        }

        public static void Assert(bool testValue ,WarningType warningType,params object[] value)
        {
            if (testValue is true) ReportWarning(warningType, value);
        }

    }
}
