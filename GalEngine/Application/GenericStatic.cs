using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalEngine
{
    public partial class GenericWindow
    {
        private static APILibrary.Win32.WindowExStyles exstyle = APILibrary.Win32.WindowExStyles.WS_EX_LAYERED;
        private static APILibrary.Win32.WindowStyles windowstyle = APILibrary.Win32.WindowStyles.WS_OVERLAPPEDWINDOW ^ APILibrary.Win32.WindowStyles.WS_SIZEBOX
            ^ APILibrary.Win32.WindowStyles.WS_MAXIMIZEBOX; 


        
    }
}
