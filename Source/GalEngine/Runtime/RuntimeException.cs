using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    static class RuntimeException
    {
        public static void Assert(bool condition, string message = null)
        {
#if DEBUG
            if (condition == true) throw new Exception(message);
#endif
        }

        public static void Assert(bool condition, Exception exception)
        {
#if DEBUG
            if (condition == true) throw exception;
#endif
        }
    }
}
