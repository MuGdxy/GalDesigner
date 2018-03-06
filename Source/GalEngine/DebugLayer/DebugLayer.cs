using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    /// <summary>
    /// Debug Layer.
    /// </summary>
    public static class DebugLayer
    {
        /// <summary>
        /// Map the error type to text.
        /// </summary>
        private static Dictionary<ErrorType, string> errorText = new Dictionary<ErrorType, string>();
        
        /// <summary>
        /// Map the warning type to text.
        /// </summary>
        private static Dictionary<WarningType, string> warningText = new Dictionary<WarningType, string>();

        /// <summary>
        /// Record the warning message.
        /// </summary>
        private static List<WarningMessage> warningList = new List<WarningMessage>();

        /// <summary>
        /// Set the params to a text.
        /// </summary>
        /// <param name="text">text.</param>
        /// <param name="value">params.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Create DebugLayer.
        /// </summary>
        static DebugLayer()
        {
            errorText.Add(ErrorType.UnknownResourceType, "Get Unknown Resource Type in resList.");
            errorText.Add(ErrorType.InconsistentResourceParameters, "Resource Parameters are inconsistent. At Line: {0}, FileName: {1}.");
            errorText.Add(ErrorType.InvalidResourceFormat, "The Resource format is not right. At Line: {0}, FileName: {1}.");
            errorText.Add(ErrorType.FileIsNotExist, "The FilePath {0} is not exist.");
            errorText.Add(ErrorType.InvalidValueType, "The value's type is not support.");
            errorText.Add(ErrorType.InvaildFileType, "The file's type is not support. At line: {0}, FileName: {1}.");
            errorText.Add(ErrorType.InvaildName, "The name is invaild in {0}.");
            errorText.Add(ErrorType.InvaildMemberValueName, "The member value's name {0} is not contained.");

            warningText.Add(WarningType.NoFramesInAnimation, "The Animation named {0} has not any KeyFrames.");
            warningText.Add(WarningType.NoTargetOfAnimation, "Can not find the target value on starting animation(Name: {0}).");
        }

        /// <summary>
        /// Transform a error to a exception.
        /// </summary>
        /// <param name="errorType">Error type.</param>
        /// <param name="value">Error params.</param>
        /// <returns></returns>
        public static Exception GetErrorException(ErrorType errorType, params object[] value)
        {
            return new Exception(SetParamsToString(errorText[errorType], value));
        }

        /// <summary>
        /// Make a error.
        /// </summary>
        /// <param name="errorType">Erorr type.</param>
        /// <param name="value">Error params.</param>
        public static void ReportError(ErrorType errorType, params object[] value)
        {
            throw GetErrorException(errorType, value);
        }

        /// <summary>
        /// Make a warning.
        /// </summary>
        /// <param name="warningType">Warning type.</param>
        /// <param name="value">Warnig params.</param>
        public static void ReportWarning(WarningType warningType, params object[] value)
        {
            var message = new WarningMessage(SetParamsToString(warningText[warningType], value), DateTime.Now);
            warningList.Add(message);

            DebugCommand.WriteCommand(message.WarningText, Internal.ResourceList.RedBrush);
        }

        /// <summary>
        /// Test value. If true then report error.
        /// </summary>
        /// <param name="testValue">Test value.</param>
        /// <param name="errorType">Error type.</param>
        /// <param name="value">Error params.</param>
        public static void Assert(bool testValue, ErrorType errorType, params object[] value)
        {
#if DEBUG
            if (testValue is true) ReportError(errorType, value);
#endif
        }

        /// <summary>
        /// Test value. If true then report warning.
        /// </summary>
        /// <param name="testValue">Test value.</param>
        /// <param name="warningType">Warning type.</param>
        /// <param name="value">Warning params.</param>
        public static void Assert(bool testValue, WarningType warningType, params object[] value)
        {
#if DEBUG
            if (testValue is true) ReportWarning(warningType, value);
#endif
        }

        /// <summary>
        /// Add a error type.
        /// </summary>
        /// <param name="errorType">The error type that you want to add.</param>
        /// <param name="messageText">The text will be maped.</param>
        public static void RegisterError(ErrorType errorType, string messageText)
        {
            errorText[errorType] = messageText;
        }

        /// <summary>
        /// Add a warning type.
        /// </summary>
        /// <param name="warningType">The warning type that you want to add.</param>
        /// <param name="messageText">The text will be maped.</param>
        public static void RegisterWarning(WarningType warningType, string messageText)
        {
            warningText[warningType] = messageText;
        }
        
        /// <summary>
        /// Get a Waring.
        /// </summary>
        /// <param name="count">The warning's ID.</param>
        /// <returns></returns>
        public static WarningMessage GetWarning(int count) => warningList[count];

        /// <summary>
        /// Get Waring List.
        /// </summary>
        public static List<WarningMessage> WarningMessageList => warningList;
    }
}
