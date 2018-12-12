using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class StringProperty
    {
        internal static string LogRuntimeNode => "Runtime";
        internal static string LogGraphicsNode => "Graphics";

        public static string Log => "[time] : ";

        public static string PackageRoot => "Package";
        public static string LogRoot => "SystemScene";
    }
}
