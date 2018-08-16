using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{

    public static partial class DebugLayer
    {
        private static Dictionary<Error, string> errors = new Dictionary<Error, string>();
        private static Dictionary<Warning, string> warnings = new Dictionary<Warning, string>();

        private static DebugCommand debugCommand = new DebugCommand();

        internal static DebugCommand DebugCommand => debugCommand;

        private static string SetParamsToString(string text, params object[] context)
        {
            string result = text;

            int count = 0;

            while (text.Contains("{" + count + "}") is true)
            {
                result = result.Replace("{" + count + "}", context[count].ToString());

                count++;
            }

            return result;
        }

        static DebugLayer()
        {
            MakeErrors();
            MakeWarnings();
        }

        public static void WriteCommandText(string Text, string Color = "Default")
        {
            DebugCommand.WriteCommandText(Text, Color);
        }

        public static void ReportError(Error error, params object[] context)
        {
            WriteCommandText(SetParamsToString(errors[error], context), Colors.Green);
        }
        
        public static void ReportWarning(Warning warning, params object[] context)
        {
            WriteCommandText(SetParamsToString(warnings[warning], context), Colors.Red);
        }

        public static void Assert(bool testValue, Error error, params object[] context)
        {
            if (testValue is true) ReportError(error, context);
        }

        public static void Assert(bool testValue, Warning warning, params object[] context)
        {
            if (testValue is true) ReportWarning(warning, context);
        }
    }
}
