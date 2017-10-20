using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class DebugLayer
    {
        private static Dictionary<ErrorType, string> errorText = new Dictionary<ErrorType, string>();
        private static Dictionary<WarningType, string> warningText = new Dictionary<WarningType, string>();

        private static List<WarningMessage> warningList = new List<WarningMessage>();

        private static List<string> watchList = new List<string>();

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
            errorText.Add(ErrorType.UnknownResourceType, "Get Unknown Resource Type in resList.");
            errorText.Add(ErrorType.InconsistentResourceParameters, "Resource Parameters are inconsistent. At Line: {0}, FileTag: {1}");
            errorText.Add(ErrorType.InvalidResourceFormat, "The Resource format is not right. At Line: {0}, FileTag: {1}");
            errorText.Add(ErrorType.FileIsNotExist, "The FilePath {0} is not exist.");
            errorText.Add(ErrorType.InvalidValueType, "The value's type is not support.");
            errorText.Add(ErrorType.InvaildFileType, "The file's type is not support. At line: {0}, FileTag: {1}");
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
            var message = new WarningMessage(SetParamsToString(warningText[warningType], value), DateTime.Now);
            warningList.Add(message);

            VisualLayer.SetPadItem(message.WarningText, message.WarningText, VisualLayer.PadType.WarningPad);
        }

        public static void Assert(bool testValue, ErrorType errorType, params object[] value)
        {
            if (testValue is true) ReportError(errorType, value);
        }

        public static void Assert(bool testValue , WarningType warningType,params object[] value)
        {
            if (testValue is true) ReportWarning(warningType, value);
        }

        public static void RegisterError(ErrorType errorType, string messageText)
        {
            errorText[errorType] = messageText;
        }

        public static void RegisterWarning(WarningType warningType, string messageText)
        {
            warningText[warningType] = messageText;
        }

        public static void AddWatch(string Tag)
        {
            watchList.Add(Tag);

            VisualLayer.SetPadItem(Tag, Tag + " = " + GlobalValue.GetValue(Tag), VisualLayer.PadType.WatchPad);
        }

        public static void RemoveWatch(string Tag)
        {
            watchList.Remove(Tag);

            VisualLayer.RemovePadItem(Tag, VisualLayer.PadType.WatchPad);
        }
        
        public static WarningMessage GetWarning(int count) => warningList[count];

        public static List<WarningMessage> WarningMessageList => warningList;

        public static List<string> WatchList => watchList;
    }
}
