using System;
using System.Numerics;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public static class Utility
    {
        public static void Dispose<T>(ref T Object) where T : class, IDisposable
        {
            if (Object == null) return;

            Object.Dispose();
            Object = null;
        }

        public static int SizeOf<T>()
        {
            return Marshal.SizeOf<T>();
        }
    }
}
