using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter
{
 

    internal static class WICHelper
    {
        private static WICTranslate translate = new WICTranslate();
        private static WICConvert convert = new WICConvert();

        public static WICTranslate Translate => translate;
        public static WICConvert Convert => convert;

    }
}
