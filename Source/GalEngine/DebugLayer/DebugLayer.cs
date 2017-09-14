﻿using System;
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
            errorText.Add(ErrorType.InconsistentResourceParameters, "Resource Parameters are inconsistent, there is current code: {0}");
            errorText.Add(ErrorType.InvalidResourceFormat, "The Resource format is not right at Line {0}, File \"{1}\"");
            errorText.Add(ErrorType.FileIsNotExist, "The FilePath {0} is not exist");
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
            warningList.Add(new WarningMessage(SetParamsToString(warningText[warningType], value), DateTime.Now));
        }

        public static void Assert(bool testValue, ErrorType errorType, params object[] value)
        {
            if (testValue is true) ReportError(errorType, value);
        }

        public static void Assert(bool testValue ,WarningType warningType,params object[] value)
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

        public static WarningMessage GetWarning(int count) => warningList[count];

    }
}
